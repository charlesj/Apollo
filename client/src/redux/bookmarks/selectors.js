import _ from "lodash";
import moment from "moment";

export function all(state) {
  const { bookmarks } = state.bookmarks;
  const array = Object.keys(state.bookmarks.bookmarks)
    .map(key => {
      return bookmarks[key];
    })
    .map(bookmark => {
      return {
        ...bookmark,
        createdAtDisplay: moment(bookmark.created_at).calendar(),
        unifiedTags: _.join(bookmark.tags, ",")
      };
    });
  return _.orderBy(array, "created_at", "desc");
}
