import path from 'path'
import moment from 'moment'
import pjson from '../package.json'

const projectName = 'USerializer'
const target = process.env.CONFIGURATION || 'Debug'
const buildNumber = process.env.APPVEYOR_BUILD_NUMBER
const appVeyorJobId = process.env.APPVEYOR_JOB_ID
const version = pjson.version
const revision = buildNumber || moment().format('HHmm')
const includeRevision = true
const versionSuffix = 'preview'
const assemblyVersion = includeRevision
  ? `${version}.${revision}`
  : `${version}.0`
const CI = process.env.CI && process.env.CI.toString().toLowerCase() === 'true'

const artifactsPath = path.resolve('./artifacts')
const solutionPath = path.resolve(`./${projectName}.sln`)
const projectSourcePath = path.resolve('./src')
const tempPath = path.resolve('./tempRelocation')
const cleanPaths = [
  tempPath,
  `${projectSourcePath}/obj`,
  `${projectSourcePath}/bin`
]

const nugetDependencies = ['UmbracoCms.Core', 'Newtonsoft.Json']

const versionInfo = {
  description: 'USerializer an EPiServer tool',
  productName: 'USerializer',
  copyright: 'Copyright 2019 Emil Olsson',
  version: assemblyVersion,
  fileVersion: assemblyVersion,
  informationalVersion: assemblyVersion
}

export default {
  projectName,
  appVeyorJobId,
  artifacts: artifactsPath,
  CI,
  cleanPaths,
  slnPath: solutionPath,
  sourcePath: projectSourcePath,
  tempPath,
  target,
  version,
  revision,
  includeRevision,
  assemblyVersion,
  taskTimeout: 120000,
  versionSuffix,
  versionInfo,
  nugetDependencies,
  assemblyInfoFilePath: './src/Properties/AssemblyInfo.cs'
}
