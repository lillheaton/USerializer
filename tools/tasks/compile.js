import exec from './exec'

export default function compile(settings) {
  const argument = `msbuild ${settings.slnPath} -property:Configuration=${
    settings.target
  }`

  return exec(argument)
}
