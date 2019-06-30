
import { logRequest } from './requestLogger'

describe('requestLogger', () => {
  it('calls next', () => {
    const req = {}
    const next = jest.fn()
    logRequest(req, undefined, next)
    expect(next).toHaveBeenCalled()
  })
})
