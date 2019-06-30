jest.mock('../../..//data/structure/connection')
jest.mock('../../http')

import { getConnection } from '../../../data/structure/connection'

import { healthCheck } from './healthCheck'
import { mockResponse, mockRequest } from '../../../tests/expressMocks'
import { ok } from '../../http'

let req = mockRequest()
let res = mockResponse()

describe('healthCheck', () => {
  it('checks for a valid database connection and returns it', async () => {
    const connection = { connected: true }
    getConnection.mockResolvedValue(connection)
    await healthCheck(req, res)
    expect(ok).toHaveBeenCalledTimes(1)
    expect(ok.mock.calls[0][1]).toMatchSnapshot()
  })
})
