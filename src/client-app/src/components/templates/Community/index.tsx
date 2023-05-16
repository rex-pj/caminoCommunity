import * as React from "react";
import { Fragment } from "react";
import CommunityItem from "../../organisms/Community/CommunityItem";
import { Pagination } from "../../organisms/Paging";
import Breadcrumb, {
  IBreadcrumbItem,
} from "../../organisms/Navigation/Breadcrumb";

type Props = {
  communities: any[];
  breadcrumbs: IBreadcrumbItem[];
  totalPage?: number;
  baseUrl: string;
  currentPage?: number;
};

const Index = (props: Props) => {
  const { communities, breadcrumbs, totalPage, baseUrl, currentPage } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <div className="row">
        {communities
          ? communities.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <CommunityItem key={item.id} community={item} />
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
};

export default Index;
