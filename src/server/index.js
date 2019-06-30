/* eslint-disable */
require('dotenv').config()
import { configureBasic, logger } from '../logging'

import { runServer } from './server'

configureBasic()
process.on('unhandledRejection', err => logger.error('app unhandled promise rejection', err))

runServer().then(() => {
  logger.info('startup complete')
}).catch((err) => {
  logger.error('ERROR starting up', err)
  process.exit(1)
})
