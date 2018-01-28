import { createStore, applyMiddleware, compose, combineReducers } from "redux";
import { routerReducer, routerMiddleware } from "react-router-redux";
import { reducer as formReducer } from "redux-form";
import thunk from "redux-thunk";
import createHistory from "history/createBrowserHistory";

import boardReducer from "./boards/reducers";
import bookmarksReducer from "./bookmarks/reducers";
import checklistsReducer from "./checklists/reducers";
import feedsReducer from "./feeds/reducers";
import goalsReducer from "./goals/reducers";
import jobsReducer from "./jobs/reducers";
import journalReducer from "./journal/reducers";
import noteReducer from "./notes/reducers";
import metaReducer from "./meta/reducers";
import metricsReducer from "./metrics/reducers";
import summariesReducer from "./summaries/reducers";

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
  boards: boardReducer,
  bookmarks: bookmarksReducer,
  checklists: checklistsReducer,
  goals: goalsReducer,
  feeds: feedsReducer,
  jobs: jobsReducer,
  journal: journalReducer,
  notes: noteReducer,
  meta: metaReducer,
  metrics: metricsReducer,
  summaries: summariesReducer,
  routing: routerReducer,
  form: formReducer
});

const store = createStore(rootReducer, composedEnhancers);

export default store;

export function getStore() {
  return store;
}

export function getDispatch() {
  return store.dispatch;
}
