import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'

const actionCreators = createActions({
  bookmarks: {
    load: basicActions(),
    save: basicActions(),
    remove: basicActions(),
  },
})

const actions = actionCreators.bookmarks

export default actions

export function load({ start, }) {
  return dispatchBasicActions(actions.load, async () => {
    const bookmarksResult = await apolloServer.invoke('getBookmarks', {
      start,
    })
    return bookmarksResult
  })
}

export function save(bookmark) {
  return dispatchBasicActions(actions.save, async () => {
    const updated = await apolloServer.invoke('saveBookmark', {
      ...bookmark,
      tags: bookmark.unifiedTags ? bookmark.unifiedTags.split(',') : [],
    })

    return { bookmark: updated, }
  })
}

export function remove(bookmark) {
  return dispatchBasicActions(actions.remove, async () => {
    await apolloServer.invoke('deleteBookmark', { id: bookmark.id, })
    return { bookmark, }
  })
}
