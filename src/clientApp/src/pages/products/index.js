import React, { Fragment } from "react";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constants";
import { useQuery } from "@apollo/client";
import { GET_PRODUCTS } from "../../utils/GraphQLQueries/queries";
import { withRouter } from "react-router-dom";

export default withRouter(function (props) {
  const { pageNumber } = props;
  const { loading, data } = useQuery(GET_PRODUCTS, {
    variables: {
      criterias: {
        page: pageNumber,
      },
    },
  });

  if (loading || !data) {
    return <Fragment></Fragment>;
  }

  const { products: productsResponse } = data;
  const { collections } = productsResponse;
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

  const baseUrl = "/products";
  const { totalPage, filter } = productsResponse;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Product",
    },
  ];

  return (
    <Product
      products={products}
      breadcrumbs={breadcrumbs}
      totalPage={totalPage}
      baseUrl={baseUrl}
      currentPage={page}
    />
  );
});
