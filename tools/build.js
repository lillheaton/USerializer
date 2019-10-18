import make from 'simple-make/lib/make'
import config from 'simple-make/lib/config'
import { log } from 'simple-make/lib/logUtils'
import {
  setVersion,
  version,
  clean,
  relocate,
  restore,
  compile,
  nuget
} from './tasks'
import settings from './settings'

const args = process.argv.slice(2)
console.log(args)

log('Settings', settings)

config.name = settings.versionInfo.productName
config.taskTimeout = settings.taskTimeout

const tasks = {
  compile: [clean, 'restore', compile],
  relocate: [relocate],
  version: [version],
  nuget: [nuget],
  restore,
  setVersion: () => setVersion(args[1]),
  ci: ['version', 'compile', 'relocate', 'nuget'],
  default: 'version'
}

make({ tasks, settings })
