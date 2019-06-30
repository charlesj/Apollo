import { clacks } from './clacks'

describe('clacks middleware', () => {
  it('sets header and calls next', () => {
    const res = { set: jest.fn() }
    const next = jest.fn()
    clacks({}, res, next)
    expect(next).toHaveBeenCalled()
    expect(res.set).toHaveBeenCalledWith('X-Clacks-Overhead', 'GNU Terry Pratchet')
  })
})
