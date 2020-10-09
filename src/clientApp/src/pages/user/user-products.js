import React, { Fragment } from "react";
import { useQuery } from "@apollo/client";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { GET_USER_PRODUCTS } from "../../utils/GraphQLQueries/queries";

export default withRouter(function (props) {
  const { location, match, pageNumber } = props;
  const { params } = match;
  const { userId } = params;

  const { loading, data } = useQuery(GET_USER_PRODUCTS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber,
      },
    },
  });

  if (loading || !data) {
    return <Fragment></Fragment>;
  }

  const { userProducts } = data;
  const { collections } = userProducts;
  const products = collections.map((item) => {
    let product = { ...item };
    product.url = `${UrlConstant.Product.url}${product.id}`;
    if (product.thumbnails) {
      const thumbnail = product.thumbnails[0];
      if (thumbnail.id > 0) {
        product.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.id}`;
      }
    }

    product.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdBy,
    };

    if (item.createdByPhotoCode) {
      product.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    return product;
  });

  const pageQuery = location.search;
  const baseUrl = props.userUrl + "/posts";
  const { totalPage, filter } = userProducts;
  const { page } = filter;

  return (
    <Fragment>
      <div className="row">
        {products
          ? products.map((item) => (
              <div
                key={item.id}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <ProductItem key={item.id} product={item} />
              </div>
            ))
          : null}
      </div>
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={page}
      />
    </Fragment>
  );
});
