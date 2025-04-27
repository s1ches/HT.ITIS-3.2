import '../../../domain/model/article.dart';
import 'model/api_article.dart';

extension ApiArticleMapper on ApiArticle {
  Article toDomain() {
    return Article(
      userId: userId,
      id: id,
      title: title,
      body: body,
    );
  }
}