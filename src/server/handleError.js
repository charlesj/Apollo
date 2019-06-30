import { logger } from '../logging'

export const handleError = (endpoint) => async (req, res, next) => {
  try {
    await endpoint(req, res, next)
  } catch (error) {
    logger.error('API Error', { method: req.method, path: req.path, message: error.message, error })
    res.status(500).json({ message: error.message }).end()
  }
}
