import express from 'express'
import { appInfo } from './meta/appInfo'
// import { healthCheck } from './meta/healthCheck'

export function getRouter() {
  const router = express.Router()

  router.get('/appInfo', appInfo)
  // router.get('/healthCheck', healthCheck)

  return router
}
