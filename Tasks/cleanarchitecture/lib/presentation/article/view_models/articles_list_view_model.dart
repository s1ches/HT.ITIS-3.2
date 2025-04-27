import 'package:flutter/material.dart';
import '../../../../domain/usecase/article/get_articles_use_case.dart';
import '../../../domain/model/article.dart';

class ArticlesListViewModel extends ChangeNotifier {
  final GetArticlesUseCase _getArticlesUseCase;

  List<Article> _articles = [];
  List<Article> get articles => _articles;

  bool _isLoading = false;
  bool get isLoading => _isLoading;

  int _currentPage = 0;
  final int _limit = 10;

  ArticlesListViewModel(this._getArticlesUseCase) {
    fetchArticles();
  }

  Future<void> fetchArticles() async {
    if (_isLoading) return;
    _isLoading = true;
    notifyListeners();

    final newArticles = await _getArticlesUseCase.execute(
      start: _currentPage * _limit,
      limit: _limit,
    );

    _articles.addAll(newArticles);
    _currentPage++;
    _isLoading = false;
    notifyListeners();
  }

  void onScrollEnd() {
    fetchArticles();
  }
}