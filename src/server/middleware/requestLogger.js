import { logger } from '../../logging'

export function logRequest(req, res, next) {
  logger.info(`${req.method} ${req.originalUrl}`)
  next()
}
