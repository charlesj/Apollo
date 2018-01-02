import { createStore, applyMiddleware, compose, combineReducers } from "redux";
import { routerReducer, routerMiddleware } from "react-router-redux";
import { reducer as formReducer } from "redux-form";
import thunk from "redux-thunk";
import createHistory from "history/createBrowserHistory";

import metaReducer from "./meta/reducers";
import metricsReducer from "./metrics/reducers";
import summariesReducer from "./summaries/reducers";
import goalsReducer from "./goals/reducers";

import { goalActions } from "./actions";

export const history = createHistory();

const enhancers = [];
const middleware = [thunk, routerMiddleware(history)];

if (process.env.NODE_ENV === "development") {
  const devToolsExtension = window.devToolsExtension;

  if (typeof devToolsExtension === "function") {
    enhancers.push(devToolsExtension());
  }
}

const composedEnhancers = compose(applyMiddleware(...middleware), ...enhancers);

const rootReducer = combineReducers({
  goals: goalsReducer,
  meta: metaReducer,
  metrics: metricsReducer,
  summaries: summariesReducer,
  routing: routerReducer,
  form: formReducer
});

const store = createStore(rootReducer, composedEnhancers);

store.dispatch(goalActions.getGoals());

export default store;

export function getStore() {
  return store;
}
