import React, { Fragment } from "react";
import AssociationItem from "../../organisms/Association/AssociationItem";
import { Pagination } from "../../organisms/Paging";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";

export default function (props) {
  const { associations, breadcrumbs, totalPage, baseUrl, currentPage } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <div className="row">
        {associations
          ? associations.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <AssociationItem key={item.id} association={item} />
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
