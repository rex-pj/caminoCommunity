import * as React from "react";
import { Fragment } from "react";
import FarmItem from "../../organisms/Farm/FarmItem";

type Props = {
  farms: any[];
  baseUrl: string;
  onOpenDeleteConfirmation: (e: any, onDeleteFunc: void) => void;
};

const Index = (props: Props) => {
  const { farms, onOpenDeleteConfirmation } = props;

  return (
    <Fragment>
      <div className="row gx-1">
        {farms
          ? farms.map((item: any, index: number) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4 px-2"
              >
                <FarmItem
                  key={item.id}
                  farm={item}
                  onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
                />
              </div>
            ))
          : null}
      </div>
    </Fragment>
  );
};

export default Index;
