export default class RunError extends Error {
  constructor(message, validationErrors) {
    super(message)
    this.validationErrors = validationErrors
  }
}
