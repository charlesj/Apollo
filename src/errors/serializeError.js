import serializeError from 'serialize-error'

function serialize(error) {
  return JSON.stringify(serializeError(error))
}

export default serialize
