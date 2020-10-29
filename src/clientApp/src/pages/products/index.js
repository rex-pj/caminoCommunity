import React from "react";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constants";
import { useQuery } from "@apollo/client";
import { GET_PRODUCTS } from "../../utils/GraphQLQueries/queries";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber } = params;
  const { loading, data, error } = useQuery(GET_PRODUCTS, {
    variables: {
      criterias: {
        page: pageNumber ? parseInt(pageNumber) : 1,
      },
    },
  });

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { products: productsResponse } = data;
  const { collections } = productsResponse;
  const products = collections.map((item) => {
    let product = { ...item };
    product.url = `${UrlConstant.Product.url}${product.id}`;
    if (product.thumbnails && product.thumbnails.length > 0) {
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

    if (product.productFarms) {
      product.productFarms = product.productFarms.map((pf) => {
        let productFarm = { ...pf };
        productFarm.url = `/farms/${pf.farmId}`;
        return productFarm;
      });
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
