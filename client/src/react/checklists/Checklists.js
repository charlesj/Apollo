import React from "react";
import { connect } from "react-redux";

import { checklistSelectors } from "../../redux/selectors";
import { checklistActions } from "../../redux/actions";
import {
  Page,
  Container,
  FlexRow,
  FlexContainer,
  SelectList,
  AddButton,
} from "../_controls";
import ChecklistForm from "./ChecklistForm";
import CompleteChecklist from "./CompleteChecklist";
import ChecklistCompletionLog from "./ChecklistCompletionLog";

import "./checklists.css";

class Checklists extends React.Component {
  componentDidMount() {
    this.props.getChecklists();
  }

  render() {
    const {
      checklists,
      removeChecklist,
      selectedChecklist,
      selectChecklist,
      saveChecklist
    } = this.props;

    return (
      <Page>
        <FlexRow>
          <FlexContainer>
            <AddButton noun="checklist" onClick={() => selectChecklist({ items: []})} />
            <Container width={200}>
              <SelectList
                items={checklists}
                onSelectItem={checklist => {
                  selectChecklist(checklist);
                }}
                labelField="name"
              />
            </Container>
          </FlexContainer>

          {selectedChecklist && (
            <FlexContainer>
              <ChecklistForm
                checklist={selectedChecklist}
                onSubmit={saveChecklist}
                onCancel={() => selectChecklist(null)}
                onDelete={() => removeChecklist(selectedChecklist)}
              />
            </FlexContainer>
          )}
          <FlexContainer>
            <CompleteChecklist />
          </FlexContainer>
          <FlexContainer>
            <ChecklistCompletionLog />
          </FlexContainer>
        </FlexRow>
      </Page>
    );
  }
}

function mapStateToProps(state, props) {
  return {
    checklists: checklistSelectors.all(state),
    selectedChecklist: checklistSelectors.selectedChecklist(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    getChecklists: () => dispatch(checklistActions.getChecklists()),
    selectChecklist: checklist =>
      dispatch(checklistActions.default.selectChecklist(checklist)),
    saveChecklist: checklist =>
      dispatch(checklistActions.saveChecklist(checklist)),
    removeChecklist: checklist =>
      dispatch(checklistActions.removeChecklist(checklist))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Checklists);
