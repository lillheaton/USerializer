import { mkdir, cp } from 'shelljs'

export default function relocate(settings) {
  const { projectName, sourcePath, tempPath } = settings
  const dllName = `${projectName}.dll`

  mkdir('-p', `${tempPath}/lib/net461`)
  cp(`${sourcePath}/bin/${dllName}`, `${tempPath}/lib/net461/${dllName}`)
  cp(`${sourcePath}/${projectName}.nuspec`, `${tempPath}/${projectName}.nuspec`)

  return Promise.resolve()
}
