import { getSetting } from './settings/getSetting'
import { setSetting } from './settings/setSetting'
import { logger } from '../logging'
import { secure } from '../security/passwords'

export const ensureDefaultData = async () => {
  try {
    const password = await getSetting('password')
    if (!password) {
      logger.info('missing default password')
      const value = secure('wokka')
      await setSetting('password', value)
    }
  } catch (err) { console.log(err) }
}
