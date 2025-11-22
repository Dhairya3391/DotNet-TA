import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  Svg: React.ComponentType<React.ComponentProps<'svg'>>;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Comprehensive .NET Labs',
    Svg: require('@site/static/img/undraw_docusaurus_mountain.svg').default,
    description: (
      <>
        Step-by-step .NET programming labs covering everything from basic concepts
        to advanced topics. Perfect for learning and mastering .NET development.
      </>
    ),
  },
  {
    title: 'Setup Made Simple',
    Svg: require('@site/static/img/undraw_docusaurus_tree.svg').default,
    description: (
      <>
        Complete setup guides for .NET SDK and VS Code. Get your development
        environment running in minutes with our detailed tutorials.
      </>
    ),
  },
  {
    title: 'Built with ❤️ by harpalll & Dhairya',
    Svg: require('@site/static/img/undraw_docusaurus_react.svg').default,
    description: (
      <>
        Complete Docusaurus integration and documentation setup by
        <a href="https://github.com/harpalll" target="_blank" rel="noopener noreferrer"> harpalll</a> &
        <a href="https://github.com/dhairya3391" target="_blank" rel="noopener noreferrer"> Dhairya</a>.
        Professional .NET documentation platform.
      </>
    ),
  },
];

function Feature({title, Svg, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <Svg className={styles.featureSvg} role="img" />
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
