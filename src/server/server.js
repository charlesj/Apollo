import express from 'express'
import cors from 'cors'
import { logger } from '../logging'
import { config } from '../config'
import { getRouter } from './api/router'
import { clacks, logRequest, removePoweredBy, finalErrorHandler, handleNotFound } from './middleware/'
// import { initialize } from '../data/structure/connection'


export async function runServer() {

  //await initialize(config.db.uri, config.db.dbName)
  // logger.info('initialized connection to mongo')

  const server = express()

  server.use(cors())
  server.use(logRequest)
  server.use(removePoweredBy)
  server.use(clacks)
  server.use(express.json())
  server.use('/', getRouter())
  server.use(handleNotFound)
  server.use(finalErrorHandler)
  logger.info('server configuration complete')

  const listeningServer = server.listen(config.server.port, () => logger.info(`Listening on ${config.server.port}`))
  return listeningServer
}
