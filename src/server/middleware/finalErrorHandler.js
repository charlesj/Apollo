import { logger } from '../../logging'

export function finalErrorHandler(err, req, res, ) {
  logger.error('Error in request', req, err)
  res.status(500).json({ error: true })
}
