import React, { Fragment } from "react";
import FarmItem from "../../organisms/Farm/FarmItem";
import { Pagination } from "../../molecules/Paging";
import loadable from "@loadable/component";

const Breadcrumb = loadable(() => import("../../molecules/Breadcrumb"));

export default function(props) {
  const { farms, breadcrumbs, totalPage, baseUrl, currentPage } = props;

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
                <FarmItem key={item.id} farm={item} />
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
