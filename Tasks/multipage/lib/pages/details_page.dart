import 'package:flutter/material.dart';

class DetailsPage extends StatelessWidget {
  final Map<String, String> data;

  const DetailsPage({super.key, required this.data});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(data['title'] ?? 'No Title'),
      ),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Text(
            data['description'] ?? 'No Description',
            style: const TextStyle(fontSize: 18),
          ),
        ),
      ),
    );
  }
}