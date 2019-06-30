import { ok } from '../../http'
import { handleError } from '../../handleError'

export const appInfo = handleError((req, res) => {
  ok(res, {
    version: '1.0.0',
  })
})



