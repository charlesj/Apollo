import _ from "lodash";
import { keyedIdToArray } from "../selector-helpers";

export function all(state) {
  return keyedIdToArray(state.checklists.checklists);
}

export function selectedChecklist(state) {
  return state.checklists.selectedChecklist;
}

export function completionLog(state) {
  return _.orderBy(
    keyedIdToArray(state.checklists.completionLog),
    "completed_at",
    "desc"
  );
}
