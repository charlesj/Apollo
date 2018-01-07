import _ from "lodash";
import moment from "moment";

import { keyedIdToArray } from "../selector-helpers";

export function all(state) {
  const { bookmarks } = state.bookmarks;
  const array = keyedIdToArray(bookmarks).map(bookmark => {
    return {
      ...bookmark,
      createdAtDisplay: moment(bookmark.created_at).calendar(),
      unifiedTags: _.join(bookmark.tags, ",")
    };
  });
  return _.orderBy(array, "created_at", "desc");
}
