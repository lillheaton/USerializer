import { mkdir, cp } from 'shelljs'

export default function relocate(settings) {
  const { projectName, sourcePath, tempPath } = settings
  const dllName = `${projectName}.dll`

  mkdir('-p', `${tempPath}/lib/net472`)
  cp(
    `${sourcePath}/bin/Release/${dllName}`,
    `${tempPath}/lib/net472/${dllName}`
  )
  cp(`${sourcePath}/${projectName}.nuspec`, `${tempPath}/${projectName}.nuspec`)

  return Promise.resolve()
}
