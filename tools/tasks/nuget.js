import exec from './exec'

export default async function compile(settings) {
  const { tempPath, projectName, target, artifacts } = settings

  const cmd = `nuget pack ${tempPath}/${projectName}.nuspec -properties Configuration=${target} -OutputDirectory ${artifacts}`

  return exec(cmd)
}
