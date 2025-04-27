import 'package:flutter/material.dart';
import '../../../domain/model/article.dart';
import '../../../domain/usecase/article/get_article_use_case.dart';
import '../../../internal/application.dart';

class SingleArticleViewModel extends ChangeNotifier {
  final GetArticleUseCase _getArticleUseCase = getIt<GetArticleUseCase>();

  Article? _article;
  Article? get article => _article;

  Future<void> fetchArticle(int id) async {
    _article = await _getArticleUseCase(id);
    notifyListeners();
  }
}