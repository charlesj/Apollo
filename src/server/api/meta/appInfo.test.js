import { appInfo } from './appInfo'
import { mockResponse, mockRequest } from '../../../tests/expressMocks'
import { ok } from '../../http'

jest.mock('../../http')

let req = mockRequest()
let res = mockResponse()
let next = jest.fn()

describe('appInfo', () => {
  it('returns expected values', () => {
    appInfo(req, res, next)
    expect(ok).toHaveBeenCalledTimes(1)
    expect(ok.mock.calls[0][0]).toBe(res)
    expect(ok.mock.calls[0][1]).toMatchSnapshot()
  })
})
