import chalk from 'chalk'
import fs from 'fs'
import updateFile from './updateFile'
import { exec } from 'child-process-promise'
import { log, logError } from 'simple-make/lib/logUtils'
import { parseString, Builder } from 'xml2js'

function gitCommit() {
  const git = 'git log -1 --pretty=format:%H'
  return exec(git)
    .then(function(result) {
      return result.stdout
    })
    .fail(function(err) {
      logError(chalk.red(err.stdout))
    })
}

export default function version(settings) {
  const { versionInfo, assemblyInfoFilePath } = settings
  return gitCommit()
    .then(commit => {
      log(`Writing ${assemblyInfoFilePath}\n`)

      const {
        version,
        fileVersion,
        informationalVersion,
        description,
        productName,
        copyright
      } = versionInfo

      const trademark = commit
      const fileInfo = `using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyDescription("${description}")]
[assembly: AssemblyTitle("${productName}")]
[assembly: AssemblyProduct("${productName}")]
[assembly: AssemblyCopyright("${copyright}")]
[assembly: AssemblyTrademark("${trademark}")]
[assembly: AssemblyVersion("${version}")]
[assembly: AssemblyFileVersion("${fileVersion}")]
[assembly: AssemblyInformationalVersion("${informationalVersion}")]

[assembly: ComVisible(false)]
[assembly: Guid("74dd5bef-1029-427b-a464-92a02daa4823")]
`

      return new Promise((resolve, reject) => {
        fs.writeFile(assemblyInfoFilePath, fileInfo, err => {
          if (err) {
            reject(err)
          } else {
            resolve()
          }
        })
      })
    })
    .then(
      updateFile(
        'Updating nuspec file version',
        './src/USerializer.nuspec',
        data => {
          let newFileContent = data

          // Read  nuspec file using xml tool
          parseString(data, (err, result) => {
            // Set nuspec file to right version
            result.package.metadata[0].version = settings.version

            // Read package.config file and sync versions
            parseString(
              fs.readFileSync('./src/packages.config', 'utf8'),
              (packageConfErr, packagedConfigResult) => {
                result.package.metadata[0].dependencies[0].dependency.forEach(
                  item => {
                    const packageRow = packagedConfigResult.packages.package.find(
                      x => x.$.id === item.$.id
                    )

                    if (!packageRow) return

                    // If we find package set item version the same as package
                    item.$.version = packageRow.$.version
                  }
                )
              }
            )

            const builder = new Builder()
            newFileContent = builder.buildObject(result)
          })

          return newFileContent
        }
      )
    )
}
