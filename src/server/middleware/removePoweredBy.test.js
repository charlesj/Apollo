import { removePoweredBy } from './removePoweredBy'

describe('removePoweredBy', () => {
  it('removes the header and calls next', () => {
    const res = {
      removeHeader: jest.fn()
    }
    const next = jest.fn()
    removePoweredBy(undefined, res, next)
    expect(next).toHaveBeenCalled()
    expect(res.removeHeader).toHaveBeenCalledWith('X-Powered-By')
  })
})
