import { createStore, applyMiddleware, compose, combineReducers, } from 'redux'
import { routerReducer, routerMiddleware, } from 'react-router-redux'
import { reducer as formReducer, } from 'redux-form'
import thunk from 'redux-thunk'
import createHistory from 'history/createBrowserHistory'

import boardReducer from './boards/reducers'
import bookmarksReducer from './bookmarks/reducers'
import checklistsReducer from './checklists/reducers'
import financialReducer from './financial/reducers'
import feedsReducer from './feeds/reducers'
import goalsReducer from './goals/reducers'
import jobsReducer from './jobs/reducers'
import journalReducer from './journal/reducers'
import noteReducer from './notes/reducers'
import metaReducer from './meta/reducers'
import metricsReducer from './metrics/reducers'
import summariesReducer from './summaries/reducers'
import timelineReducer from './timeline/reducers'
import userSettingsReducer from './userSettings/reducers'

export const history = createHistory()

const enhancers = []
const middleware = [thunk, routerMiddleware(history),]

if (process.env.NODE_ENV === 'development') {  // eslint-disable-line no-undef
  const devToolsExtension = window.devToolsExtension

  if (typeof devToolsExtension === 'function') {
    enhancers.push(devToolsExtension())
  }
}

const composedEnhancers = compose(applyMiddleware(...middleware), ...enhancers)

const rootReducer = combineReducers({
  boards: boardReducer,
  bookmarks: bookmarksReducer,
  checklists: checklistsReducer,
  financial: financialReducer,
  feeds: feedsReducer,
  goals: goalsReducer,
  jobs: jobsReducer,
  journal: journalReducer,
  notes: noteReducer,
  meta: metaReducer,
  metrics: metricsReducer,
  summaries: summariesReducer,
  routing: routerReducer,
  form: formReducer,
  timeline: timelineReducer,
  userSettings: userSettingsReducer,
})

const store = createStore(rootReducer, composedEnhancers)

export default store

export function getStore() {
  return store
}

export function getDispatch() {
  return store.dispatch
}
