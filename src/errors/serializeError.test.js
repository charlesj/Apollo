import serializer from './serializeError'

describe('error serializer', () => {

  it('gives useful information for errors', () => {
    const error = new Error('I am error message')
    const serialized = serializer(error)

    expect(typeof serialized).toEqual('string')
    // hard to find the best way to test this that isn't brittle
    expect(serialized.indexOf('"name":"Error","message":"I am error message"') > 0).toBe(true)
  })

  it('also handles regular objects', () => {
    const error = { name: 'I am error' }
    const serialized = serializer(error)

    expect(typeof serialized).toEqual('string')
    // hard to find the best way to test this that isn't brittle
    expect(serialized.indexOf('"name":"I am error"') > 0).toBe(true)
  })
})
