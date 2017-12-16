export function basicActions() {
  return {
    start: () => null,
    complete: payload => payload,
    fail: err => err
  };
}

export function dispatchBasicActions(action, funcMap) {
  return async dispatch => {
    let completeFunc;
    let payloadFunc;
    let failFunc;

    if (typeof funcMap === "function") {
      payloadFunc = funcMap;
    } else {
      completeFunc = funcMap.complete;
      payloadFunc = funcMap.payload;
      failFunc = funcMap.fail;
    }

    try {
      dispatch(action.start());
      const payload = await payloadFunc(dispatch);
      dispatch(action.complete(payload));
      if (completeFunc) {
        completeFunc(dispatch, payload);
      }
      return payload;
    } catch (err) {
      dispatch(action.fail(err));
      if (failFunc) {
        failFunc(dispatch, err);
      }
    }
  };
}

export function basicState() {
  return {
    error: null,
    isFetching: false,
    isLoaded: false
  };
}

export function basicCompleteReducer(state, action) {
  return {
    ...state,
    error: null,
    isFetching: false
  };
}

export function basicLoadCompleteReducer(state, action) {
  return {
    ...basicCompleteReducer(state, action),
    isLoaded: true
  };
}

export function basicFailReducer(state, action) {
  return {
    ...state,
    error: action.payload,
    isFetching: false
  };
}

export function basicStartReducer(state, action) {
  return {
    ...state,
    error: null,
    isFetching: true
  };
}
