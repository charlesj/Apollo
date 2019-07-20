import express from 'express'
import { appInfo } from './meta/appInfo'
import { healthCheck } from './meta/healthCheck'
import { handleError } from '../handleError'

export function getRouter() {
  const router = express.Router()

  router.get('/appInfo', handleError(appInfo))
  router.get('/healthCheck', handleError(healthCheck))

  return router
}
