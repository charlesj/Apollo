export default class RunError extends Error {
  constructor(userMessage, data = {}, privateDescription = userMessage) {
    super(userMessage)
    this.privateDescription = privateDescription
    this.data = data
  }
}
