
export const HttpConstants = {
  BadRequestName: 'BadRequest',
  NotFoundName: 'NotFound',
  NotFoundMessage: 'Resource not found',
  ServerErrorMessage: 'The server encountered an error processing the request',
  ServerErrorName: 'InternalServerError',
  UnauthorizedMessage: 'The user is not authorized to do this',
  UnauthorizedName: 'Unauthorized',
}

export function accepted(res, message) {
  res.status(202).json({ message })
}

export function badRequest(res, errorMessage, validationErrors) {
  res.status(400).json({
    name: HttpConstants.BadRequestName,
    errorMessage,
    validationErrors,
  })
}

export function created(res, entity) {
  res.status(201).json(entity)
}

export function noContent(res) {
  res.status(204).end()
}

export function notFound(res) {
  res.status(404).json({
    message: HttpConstants.NotFoundMessage,
    name: HttpConstants.NotFoundName,
  })
}

export function ok(res, entity) {
  res.status(200).json(entity)
}

export function serverError(res) {
  res.status(500).json({
    message: HttpConstants.ServerErrorMessage,
    name: HttpConstants.ServerErrorName,
  })
}


export function unauthorized(res) {
  res.status(401).json({
    message: HttpConstants.UnauthorizedMessage,
    name: HttpConstants.UnauthorizedName,
  })
}
