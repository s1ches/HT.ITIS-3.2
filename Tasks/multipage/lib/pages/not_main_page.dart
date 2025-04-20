import 'package:flutter/material.dart';
import '../common/components/drawer.dart';

class SecondaryPage extends StatelessWidget {
  const SecondaryPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: const MainDrawer(),
      appBar: AppBar(
        title: const Text('Not Main Page'),
      ),
      body: const Center(
        child: Padding(
          padding: EdgeInsets.all(16.0),
          child: Text(
            'Может в мобильщика переквалифицироваться, вёрстка куда удобнее, чем на том же html, css.',
            style: TextStyle(fontSize: 18),
          ),
        ),
      ),
    );
  }
}