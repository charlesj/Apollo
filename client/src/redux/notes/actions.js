import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  notebooks: {
    getAll: basicActions(),
    saveNote: basicActions()
  }
});

const actions = actionCreators.notebooks;

export default actions;

export function getAll() {
  return dispatchBasicActions(actions.getAll, async () => {
    const notes = await apolloServer.invoke("GetNotes", {});

    if (Array.isArray(notes)) {
      return { notes };
    } else {
      return { notes: [] };
    }
  });
}

export function saveNote(note) {
  return dispatchBasicActions(actions.saveNote, async () => {
    const updated = await apolloServer.invoke("upsertNote", note);
    return { note: updated };
  });
}
