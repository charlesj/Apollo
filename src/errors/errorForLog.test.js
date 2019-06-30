
import { errorForLog } from './errorForLog'

describe('errorForLog', () => {
  it('converts a top level error', () => {
    const error = new Error('Top level error')
    const cleaned = errorForLog(error)
    expect(Object.keys(cleaned)).toMatchSnapshot()
  })

  it('converts a nested error', () => {
    const error = new Error('Top level error')
    const cleaned = errorForLog({ error })
    expect(Object.keys(cleaned.error)).toMatchSnapshot()
  })

  it('converts an error with an error', () => {
    const error = new Error('Top level error')
    const cleaned = errorForLog([{ error }])
    expect(Object.keys(cleaned[0].error)).toMatchSnapshot()
  })

  it('converts an array into an object', () => {
    const cleaned = errorForLog(['one', { nested: true }])
    expect(cleaned).toMatchSnapshot()
  })
})
