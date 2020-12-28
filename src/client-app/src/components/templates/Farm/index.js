import React, { Fragment } from "react";
import FarmItem from "../../organisms/Farm/FarmItem";
import { Pagination } from "../../organisms/Paging";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";

export default function (props) {
  const {
    farms,
    breadcrumbs,
    totalPage,
    baseUrl,
    currentPage,
    onOpenDeleteConfirmation,
  } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <div className="row">
        {farms
          ? farms.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
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
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    </Fragment>
  );
}
