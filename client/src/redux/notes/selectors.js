import _ from "lodash";

export function all(state) {
  return _.orderBy(state.notes.notes, "created_at", "desc");
}
