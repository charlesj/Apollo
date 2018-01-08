import _ from "lodash";
import moment from "moment";

import { keyedIdToArray } from "../selector-helpers";

export function all(state) {
  const { entries } = state.log;
  const array = keyedIdToArray(entries).map(entry => {
    return {
      ...entry,
      createdAtDisplay: moment(entry.created_at).calendar()
    };
  });
  return _.orderBy(array, "created_at", "desc");
}
