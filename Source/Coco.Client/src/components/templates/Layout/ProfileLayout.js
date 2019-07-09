import React, { useState } from "react";
import FrameLayout from "./FrameLayout";
import styled from "styled-components";
import DefaultModal from "../../atoms/Modals/DefaultModal";

const Wrapper = styled.div`
  > .row {
    margin-left: -8px;
    margin-right: -8px;
  }

  > .row > .col {
    padding: 0 8px;
  }
`;

export default ({ component: Component, ...rest }) => {
  const [isOpenModal, setModalOpen] = useState(false);
  function openModalHandler() {
    setModalOpen(true);
  }

  function closeModalHandler() {
    setModalOpen(false);
  }

  return (
    <FrameLayout
      {...rest}
      component={matchProps => (
        <Wrapper className="container px-lg-5">
          <Component {...matchProps} />
          <DefaultModal
            isOpen={isOpenModal}
            openModalHandler={openModalHandler}
            closeModalHandler={closeModalHandler}
          />
        </Wrapper>
      )}
    />
  );
};
