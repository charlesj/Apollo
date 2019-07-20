import { ok } from '../../http'

export const appInfo = (req, res) => {
  ok(res, {
    version: '1.0.0',
  })
}



